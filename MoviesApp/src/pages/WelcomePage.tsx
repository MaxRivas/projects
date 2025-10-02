import Button from '@/components/ui/Button';

type Props = {
  onShowMovies?: () => void;
};

export default function WelcomePage({ onShowMovies }: Props) {
  return (
    <div style={{ padding: '2rem' }}>
      <h2>Welcome to Movies Application.</h2>
      <div style={{ marginTop: '1rem' }}>
        <Button onClick={onShowMovies}>Movies</Button>
      </div>
    </div>
  );
}
