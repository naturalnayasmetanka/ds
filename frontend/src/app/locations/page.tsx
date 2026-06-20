import { LocationsList } from "@/components/locations/list/locations.list";

export default function LocationsPage() {
  return (
    <div className="container mx-auto max-w-6xl px-4 py-8">
      <div className="mb-6 flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
        <div>
          <h1 className="text-2xl font-semibold tracking-tight">Локации</h1>
          <p className="text-sm text-muted-foreground">
            Управление точками обслуживания и их адресами
          </p>
        </div>
      </div>

      <LocationsList />
    </div>
  );
}
