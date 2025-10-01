import React from 'react';
import Button from '@/components/ui/Button';

type Props = {
  onShowMovies?: () => void;
};

export default function WelcomePage({ onShowMovies }: Props) {
  return (
    <div style={{ padding: '2rem' }}>
      <h2>Welcome to my app</h2>
      <div style={{ marginTop: '1rem' }}>
        <Button onClick={() => onShowMovies ? onShowMovies() : alert('Hello from shadcn-style Button!')}>Click me</Button>
      </div>
    </div>
  );
}
