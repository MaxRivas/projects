import React, { createContext, useContext, useEffect, useRef, useState } from "react";

type TokenResponse = {
  access_token: string;
  refresh_token?: string;
  expires_in?: number;
};

type AuthState = {
  token: string | null;
  refreshToken?: string | null;
  user?: string | null;
};

type AuthContextValue = {
  token: string | null;
  user?: string | null;
  // changed: return success + optional message so callers can show server response body on failure
  login: (username: string, password: string) => Promise<{ success: boolean; message?: string }>;
  logout: () => void;
  register: (username: string, password: string) => Promise<boolean>;
  refresh: () => Promise<boolean>;
  authFetch: (input: RequestInfo, init?: RequestInit) => Promise<Response>;
};

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

const STORAGE_TOKEN = "auth.token";
const STORAGE_REFRESH = "auth.refresh";
const STORAGE_USER = "auth.user";

function decodeJwtPayload(token: string): any | null {
  try {
    const parts = token.split(".");
    if (parts.length < 2) return null;
    const payload = parts[1].replace(/-/g, "+").replace(/_/g, "/");
    const json = decodeURIComponent(
      atob(payload)
        .split("")
        .map((c) => "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2))
        .join("")
    );
    return JSON.parse(json);
  } catch {
    return null;
  }
}

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [state, setState] = useState<AuthState>(() => ({
    token: localStorage.getItem(STORAGE_TOKEN),
    refreshToken: localStorage.getItem(STORAGE_REFRESH),
    user: localStorage.getItem(STORAGE_USER),
  }));
  const refreshTimer = useRef<number | null>(null);

  // Minimal effect: do NOT auto-login. If a token already exists, schedule refresh.
  useEffect(() => {
    if (state.token) scheduleRefresh(state.token, state.refreshToken);
    return () => {
      if (refreshTimer.current) window.clearTimeout(refreshTimer.current);
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  function persist(s: AuthState) {
    if (s.token) localStorage.setItem(STORAGE_TOKEN, s.token);
    else localStorage.removeItem(STORAGE_TOKEN);
    if (s.refreshToken) localStorage.setItem(STORAGE_REFRESH, s.refreshToken);
    else localStorage.removeItem(STORAGE_REFRESH);
    if (s.user) localStorage.setItem(STORAGE_USER, s.user);
    else localStorage.removeItem(STORAGE_USER);
    setState(s);
  }

  function scheduleRefresh(token: string, _refreshToken?: string | null) {
    if (refreshTimer.current) {
      window.clearTimeout(refreshTimer.current);
      refreshTimer.current = null;
    }
    const payload = decodeJwtPayload(token);
    if (!payload || !payload.exp) return;
    const expiresAt = payload.exp * 1000;
    const now = Date.now();
    const refreshAt = Math.max(now + 500, expiresAt - 60_000);
    const ms = Math.max(0, refreshAt - now);
    refreshTimer.current = window.setTimeout(async () => {
      const ok = await refresh();
      if (!ok) logout();
    }, ms) as unknown as number;
  }

  async function requestTokenEndpoint(
    url: string,
    body: Record<string, unknown>
  ): Promise<TokenResponse | { status: number; body?: any } | null> {
    try {
      const res = await fetch(url, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(body),
      });
      const txt = await res.text();
      let json: any = null;
      try {
        json = txt ? JSON.parse(txt) : null;
      } catch {
        json = txt;
      }
      if (res.ok) return json as TokenResponse;
      return { status: res.status, body: json };
    } catch (err) {
      console.debug("requestTokenEndpoint fetch error", err);
      return null;
    }
  }

  // Try typical endpoints (Vite proxies "/MoviesApi" to backend in dev)
  const tokenEndpoints = ["/MoviesApi/Auth/token", "/Auth/token", "/token"];

  async function login(username: string, password: string) {
    for (const ep of tokenEndpoints) {
      const result = await requestTokenEndpoint(ep, { username, password });
      if (!result) continue;
      if (typeof result === "object" && "access_token" in result) {
        const tokenResp = result as TokenResponse;
        persist({ token: tokenResp.access_token, refreshToken: tokenResp.refresh_token ?? null, user: username });
        scheduleRefresh(tokenResp.access_token, tokenResp.refresh_token ?? null);
        return { success: true as const };
      }
      // got non-ok response (diagnostic)
      console.warn("Auth login attempt failed:", ep, result);
      // if 401 return immediately with server body/message for UI
      if ((result as any)?.status === 401) {
        const body = (result as any)?.body;
        const message =
          typeof body === "string"
            ? body
            : body && (body.error || body.message || JSON.stringify(body));
        return { success: false, message: message ?? "Unauthorized" };
      }
    }
    // no endpoint returned a token
    return { success: false, message: "Token request failed (no response)" };
  }

  async function register(): Promise<boolean> {
    // similar to login; omitted for brevity
    return false;
  }

  async function refresh(): Promise<boolean> {
    const rt = state.refreshToken;
    if (!rt) return false;
    const endpoints = ["/MoviesApi/Auth/refresh", "/Auth/refresh", "/refresh"];
    for (const ep of endpoints) {
      const r = await requestTokenEndpoint(ep, { refresh_token: rt });
      if (r && typeof r === "object" && "access_token" in r) {
        const tokenResp = r as TokenResponse;
        persist({ token: tokenResp.access_token, refreshToken: tokenResp.refresh_token ?? null, user: state.user ?? null });
        scheduleRefresh(tokenResp.access_token, tokenResp.refresh_token ?? null);
        return true;
      }
    }
    return false;
  }

  function logout() {
    if (refreshTimer.current) {
      window.clearTimeout(refreshTimer.current);
      refreshTimer.current = null;
    }
    persist({ token: null, refreshToken: null, user: null });
  }

  async function authFetch(input: RequestInfo, init?: RequestInit) {
    const headers = new Headers(init?.headers ?? {});
    if (state.token) headers.set("Authorization", `Bearer ${state.token}`);
    const res = await fetch(input, { ...(init ?? {}), headers });
    // auto-logout on 401 response
    if (res.status === 401) {
      console.debug("AuthFetch received 401, logging out");
      logout();
    }
    return res;
  }

  const value: AuthContextValue = {
    token: state.token ?? null,
    user: state.user ?? undefined,
    login,
    logout,
    register,
    refresh,
    authFetch,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used within an AuthProvider");
  return ctx;
}