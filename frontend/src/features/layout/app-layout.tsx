"use client";

import Header from "@/components/header/header";
import Footer from "@/components/footer/footer";
import { QueryClientProvider } from "@tanstack/react-query";
import { queryClient } from "@/shared/query.client";

export default function Layout({
  children,
}: Readonly<{ children: React.ReactNode }>) {
  return (
    <QueryClientProvider client={queryClient}>
      <Header />
      <main className="flex-1 bg-stone-50">{children}</main>
      <Footer />
    </QueryClientProvider>
  );
}
