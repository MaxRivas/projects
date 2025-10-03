import React, { useEffect, useState } from "react";
import "./App.css";

// Import namespaces to avoid assuming default export shape
import * as LoginPageModule from "./pages/LoginPage";
import * as WelcomePageModule from "./pages/WelcomePage";
import * as MoviesListModule from "./components/MoviesList";

/* Resolve component exports robustly:
   prefer `default`, then common named exports used in this repo (CardDemo, LoginPage, WelcomePage, MoviesList).
   Provide a safe fallback noop component to avoid transform/runtime failures.
*/
const resolveDefault =
  (mod: any, names: string[]) =>
    (mod && (mod.default ?? names.map(n => mod[n]).find(Boolean))) ??
    (() => null);

type MovieDto = {
  id: number;
  title: string;
  director: string;
  genre: string;
  releaseYear: number;
};

type LoginProps = { onLogin: (u: string, p: string) => Promise<boolean> };
type WelcomeProps = { onShowMovies: () => void };
type MoviesListProps = { movies: MovieDto[] };

const LoginComp = resolveDefault(LoginPageModule, ["CardDemo", "LoginPage"]) as React.ComponentType<LoginProps>;
const WelcomeComp = resolveDefault(WelcomePageModule, ["WelcomePage"]) as React.ComponentType<WelcomeProps>;
const MoviesListComp = resolveDefault(MoviesListModule, ["MoviesList"]) as React.ComponentType<MoviesListProps>;

function App() {
  const [movies, setMovies] = useState<MovieDto[]>([]);
  const [route, setRoute] = useState<"login" | "welcome" | "movies">("login");
  const [token, setToken] = useState<string | null>(null);

  async function requestToken(creds?: { username: string; password: string }): Promise<string | null> {
    try {
      const body = creds ?? { username: "dev", password: "dev" };
      const res = await fetch("/MoviesApi/Auth/token", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(body),
      });
      if (!res.ok) return null;
      const data = await res.json();
      return data?.access_token ?? data?.token ?? null;
    } catch {
      return null;
    }
  }

  // Accept optional explicitToken to avoid race with setToken
  async function fetchWithAuth(input: RequestInfo, init?: RequestInit, explicitToken?: string) {
    const headers = new Headers(init?.headers ?? {});
    const bearer = explicitToken ?? token;
    if (bearer) headers.set("Authorization", `Bearer ${bearer}`);
    return fetch(input, { ...(init ?? {}), headers });
  }

  async function prefillAndLoadMovies(activeToken?: string) {
    const t = activeToken ?? token;
    if (!t) return;
    try {
      await fetchWithAuth("/MoviesApi/Prefill", undefined, t);
    } catch (e) {
      console.warn("Prefill failed", e);
    }
    try {
      const res = await fetchWithAuth("/MoviesApi/Movies", undefined, t);
      if (res.ok) {
        const data = await res.json();
        setMovies(data ?? []);
      } else {
        setMovies([]);
      }
    } catch {
      setMovies([]);
    }
  }

  // Attempt a silent/dev token on mount (optional)
  useEffect(() => {
    (async () => {
      const devToken = await requestToken();
      if (devToken) {
        setToken(devToken);
        await prefillAndLoadMovies(devToken);
        setRoute("movies");
      }
    })();
  }, []);

  async function handleLogin(username: string, password: string) {
    const t = await requestToken({ username, password });
    if (!t) return false;
    setToken(t);
    await prefillAndLoadMovies(t);
    setRoute("movies");
    return true;
  }

  return (
    <main>
      {route === "login" ? (
        <LoginComp onLogin={handleLogin} />
      ) : route === "welcome" ? (
        <WelcomeComp onShowMovies={() => setRoute("movies")} />
      ) : (
        <MoviesListComp movies={movies} />
      )}
    </main>
  );
}

export default App;
