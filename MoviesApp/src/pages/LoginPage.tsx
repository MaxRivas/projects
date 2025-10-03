import React, { useState } from "react";
import { Button } from "../components/ui/button";
import { Input } from "../components/ui/input";
import { Label } from "../components/ui/label";

import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "../components/ui/card";

import { useAuth } from "../contexts/AuthProvider";

export function LoginComp({
  onLogin,
}: {
  onLogin?: (u: string, p: string) => Promise<boolean>;
}) {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const auth = useAuth();

  async function submit(e?: React.FormEvent) {
    e?.preventDefault();
    setError(null);

    // basic validation
    if (!username.trim() || !password.trim()) {
      setError("Username and password are required.");
      return;
    }

    setLoading(true);
    try {
      const res = await auth.login(username.trim(), password);
      if (!res.success) {
        // show server response body / message visibly to the user
        setError(res.message ?? "Invalid credentials or token request failed.");
        return;
      }

      // keep backwards-compatible optional parent callback (if present)
      if (onLogin) {
        try {
          await onLogin(username.trim(), password);
        } catch {
          // ignore
        }
      }
    } finally {
      setLoading(false);
    }
  }

  return (
    <Card className="w-full max-w-sm">
      <CardHeader>
        <CardTitle>Login to your account</CardTitle>
        <CardDescription>
          Enter user name and password below to login to your account
        </CardDescription>
      </CardHeader>
      <CardContent>
        <form onSubmit={submit}>
          <div className="flex flex-col gap-2">
            <div className="grid gap-2">
              <Label htmlFor="username">User Name</Label>
              <Input
                id="username"
                type="text"
                placeholder="some user"
                required
                value={username}
                onChange={(e) => setUsername(e.target.value)}
              />
            </div>
            <div className="grid gap-2">
              <div className="flex items-center">
                <Label htmlFor="password">Password</Label>
                <a
                  href="#"
                  className="ml-auto inline-block text-sm underline-offset-4 hover:underline"
                >
                  Forgot your password?
                </a>
              </div>
              <Input
                id="password"
                type="password"
                required
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </div>
          </div>
        </form>
        {error && <div className="mt-2 text-sm text-red-600" role="alert">{error}</div>}
      </CardContent>
      <CardFooter className="flex-col gap-2">
        <Button type="submit" className="w-full" disabled={loading} onClick={submit}>
          {loading ? "Signing in…" : "Login"}
        </Button>
      </CardFooter>
    </Card>
  );
}

export default LoginComp;
