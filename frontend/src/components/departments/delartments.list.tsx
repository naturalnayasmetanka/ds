"use client";

import { departmentsAPI } from "@/entities/departments/api";
import { useQuery } from "@tanstack/react-query";
import { useState } from "react";
import { Spinner } from "../ui/spinner";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import {
  Pagination,
  PaginationContent,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
} from "@/components/ui/pagination";
import { format } from "date-fns";
const PAGE_SIZE = 2;

export default function DepartmentsList() {
  const [page, setPage] = useState<number>(1);
  const {
    data: departmentsData,
    isLoading,
    error,
  } = useQuery({
    queryFn: () =>
      departmentsAPI.getDepartments({ pageNumber: page, pageSize: PAGE_SIZE }),
    queryKey: ["departments", page, PAGE_SIZE],
  });

  if (isLoading) {
    return (
      <div className="flex items-center justify-center py-16">
        <Spinner />
      </div>
    );
  }

  if (error) {
    return (
      <div className="rounded-md border border-destructive/50 bg-destructive/10 p-4 text-sm text-destructive">
        {error.message}
      </div>
    );
  }

  const departments = departmentsData?.items ?? [];

  return (
    <div className="space-y-4">
      <div className="rounded-md border">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Название</TableHead>
              <TableHead>Путь</TableHead>
              <TableHead>Создан</TableHead>
              <TableHead>Обновлён</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {departments.length === 0 ? (
              <TableRow>
                <TableCell
                  colSpan={4}
                  className="h-24 text-center text-muted-foreground"
                >
                  Отделы не найдены
                </TableCell>
              </TableRow>
            ) : (
              departments.map((department) => (
                <TableRow key={department.id}>
                  <TableCell className="font-medium">
                    {department.name}
                  </TableCell>
                  <TableCell className="text-muted-foreground">
                    {department.path}
                  </TableCell>
                  <TableCell>
                    {format(new Date(department.createdAt), "dd.MM.yyyy")}
                  </TableCell>
                  <TableCell>
                    {format(new Date(department.updatedAt), "dd.MM.yyyy")}
                  </TableCell>
                </TableRow>
              ))
            )}
          </TableBody>
        </Table>
      </div>

      {departmentsData && departmentsData.totalCount > 0 && (
        <div className="flex items-center justify-between">
          <p className="text-sm text-muted-foreground">
            Страница {departmentsData.pageNumber} из{" "}
            {departmentsData.totalPages} · всего {departmentsData.totalCount}
          </p>

          <Pagination className="mx-0 w-auto">
            <PaginationContent>
              <PaginationItem>
                <PaginationPrevious
                  href="#"
                  onClick={(e) => {
                    e.preventDefault();
                    if (departmentsData.hasPreviousPage) setPage((p) => p - 1);
                  }}
                  className={
                    !departmentsData.hasPreviousPage
                      ? "pointer-events-none opacity-50"
                      : undefined
                  }
                />
              </PaginationItem>

              {Array.from(
                { length: departmentsData.totalPages },
                (_, i) => i + 1,
              ).map((p) => (
                <PaginationItem key={p}>
                  <PaginationLink
                    href="#"
                    isActive={p === departmentsData.pageNumber}
                    onClick={(e) => {
                      e.preventDefault();
                      setPage(p);
                    }}
                  >
                    {p}
                  </PaginationLink>
                </PaginationItem>
              ))}

              <PaginationItem>
                <PaginationNext
                  href="#"
                  onClick={(e) => {
                    e.preventDefault();
                    if (departmentsData.hasNextPage) setPage((p) => p + 1);
                  }}
                  className={
                    !departmentsData.hasNextPage
                      ? "pointer-events-none opacity-50"
                      : undefined
                  }
                />
              </PaginationItem>
            </PaginationContent>
          </Pagination>
        </div>
      )}
    </div>
  );
}
