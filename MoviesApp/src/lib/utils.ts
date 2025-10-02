// Simple className helper. Keep it minimal to avoid extra deps during build.
export function cn(...inputs: Array<string | false | null | undefined>) {
  return inputs.filter(Boolean).join(' ');
}
