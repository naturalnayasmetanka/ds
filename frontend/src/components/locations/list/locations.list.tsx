"use client";

import { useState } from "react";
import {
  Clock,
  Inbox,
  MapPin,
  MoreHorizontal,
  Plus,
  Search,
} from "lucide-react";

import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardFooter,
  CardHeader,
} from "@/components/ui/card";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Input } from "@/components/ui/input";
import { Tabs, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { locaitonsAPI } from "@/entities/locations/api";
import { Spinner } from "@/components/ui/spinner";
import { useQuery } from "@tanstack/react-query";
import CreateLocationDialog from "@/features/locations/create-location-dialog";

type StatusFilter = "all" | "active" | "inactive";

export function LocationsList() {
  const [isDialogOpen, setDialogOpen] = useState<boolean>(false);
  const [status, setStatus] = useState<StatusFilter>("all");
  const [query, setQuery] = useState("");
  const {
    data: locationsData,
    isLoading,
    error,
  } = useQuery({
    queryFn: () => locaitonsAPI.getLocations(),
    queryKey: ["locations"],
  });

  if (isLoading) {
    return <Spinner />;
  }

  if (error) {
    return <div>{error.message}</div>;
  }

  return (
    <div className="space-y-4">
      <Button onClick={() => setDialogOpen(true)}>
        <Plus className="mr-2 h-4 w-4" />
        Добавить локацию
      </Button>
      <div className="flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
        <div className="relative w-full sm:max-w-sm">
          <Search className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground" />
          <Input
            value={query}
            onChange={(event) => setQuery(event.target.value)}
            placeholder="Поиск по названию или адресу"
            className="pl-9"
          />
        </div>

        <Tabs
          value={status}
          onValueChange={(value) => setStatus(value as StatusFilter)}
        >
          <TabsList>
            <TabsTrigger value="all">Все</TabsTrigger>
            <TabsTrigger value="active">Активные</TabsTrigger>
            <TabsTrigger value="inactive">Неактивные</TabsTrigger>
          </TabsList>
        </Tabs>
      </div>

      {locationsData == undefined ? (
        <EmptyState hasQuery={query.length > 0} />
      ) : (
        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
          {locationsData.map((location) => (
            <LocationCard key={location.id} location={location} />
          ))}
        </div>
      )}

      <CreateLocationDialog
        isDialogOpen={isDialogOpen}
        setDialogOpen={setDialogOpen}
      />
    </div>
  );
}

function LocationCard({ location }: { location: LocationDTO }) {
  return (
    <Card className="flex flex-col">
      <CardHeader className="flex flex-row items-start justify-between gap-2 pb-3">
        <div className="min-w-0">
          <h3 className="truncate text-base font-semibold leading-tight">
            {location.name}
          </h3>
          <p className="mt-1 text-xs text-muted-foreground">
            {location.address.settlementType} {location.address.settlementName}
          </p>
        </div>
        <Badge
          variant={location.isActive ? "default" : "secondary"}
          className="shrink-0"
        >
          {location.isActive ? "Активна" : "Неактивна"}
        </Badge>
      </CardHeader>

      <CardContent className="flex-1 space-y-2.5 pb-3">
        <div className="flex items-start gap-2 text-sm">
          <MapPin className="mt-0.5 h-4 w-4 shrink-0 text-muted-foreground" />
          <span className="text-foreground/90">
            {location.address.fullAddress}
          </span>
        </div>
        <div className="flex items-center gap-2 text-sm text-muted-foreground">
          <Clock className="h-4 w-4 shrink-0" />
          <span>{location.timezone}</span>
        </div>
        {location.address.comment ? (
          <p className="rounded-md bg-muted px-2.5 py-1.5 text-xs text-muted-foreground">
            {location.address.comment}
          </p>
        ) : null}
      </CardContent>

      <CardFooter className="flex items-center justify-between border-t pt-3 text-xs text-muted-foreground">
        <DropdownMenu>
          <DropdownMenuTrigger
            render={<Button variant="ghost" size="icon" className="h-7 w-7" />}
          >
            <MoreHorizontal className="h-4 w-4" />
            <span className="sr-only">Действия</span>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end">
            <DropdownMenuItem>Открыть</DropdownMenuItem>
            <DropdownMenuItem>Редактировать</DropdownMenuItem>
            <DropdownMenuSeparator />
            <DropdownMenuItem className="text-destructive">
              {location.isActive ? "Деактивировать" : "Активировать"}
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      </CardFooter>
    </Card>
  );
}

function EmptyState({ hasQuery }: { hasQuery: boolean }) {
  return (
    <div className="flex flex-col items-center justify-center gap-3 rounded-lg border border-dashed py-16 text-center">
      <Inbox className="h-8 w-8 text-muted-foreground" />
      <div>
        <p className="font-medium">Локации не найдены</p>
        <p className="text-sm text-muted-foreground">
          {hasQuery
            ? "Попробуйте изменить запрос или сбросить фильтр"
            : "Добавьте первую локацию, чтобы она появилась здесь"}
        </p>
      </div>
    </div>
  );
}
