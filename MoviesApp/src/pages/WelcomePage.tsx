import React from 'react';
import Button from '@/components/ui/Button';

export default function WelcomePage() {
  return (
    <div style={{ padding: '2rem' }}>
      <h2>Welcome to my app</h2>
      <p>This is a small demo app that lists movies fetched from the backend API.</p>
      <div style={{ marginTop: '1rem' }}>
        <Button onClick={() => alert('Hello from shadcn-style Button!')}>Click me</Button>
      </div>
    </div>
  );
}
