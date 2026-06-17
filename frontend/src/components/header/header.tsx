"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { ChevronDown, LogOut, Settings, User } from "lucide-react";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { routes } from "@/shared/routes";

const navItems = [
  {
    href: routes.departments,
    label: "Отделы",
    accent: "text-indigo-600",
    dot: "bg-indigo-500",
  },
  {
    href: routes.locations,
    label: "Локации",
    accent: "text-emerald-600",
    dot: "bg-emerald-500",
  },
  {
    href: routes.positions,
    label: "Должности",
    accent: "text-amber-600",
    dot: "bg-amber-500",
  },
];

// Клиентский компонент: usePathname и DropdownMenu требуют "use client",
// в отличие от статической главной страницы.
export default function Header() {
  const pathname = usePathname();

  return (
    <header className="sticky top-0 z-40 border-b border-stone-200 bg-white/80 backdrop-blur">
      <div className="mx-auto flex h-16 max-w-5xl items-center justify-between px-6">
        <Link href={routes.home} className="flex items-center gap-2.5">
          <span className="flex h-8 w-8 items-center justify-center rounded-lg bg-stone-900 text-sm font-semibold text-white">
            М
          </span>
          <span className="text-sm font-semibold tracking-tight text-stone-900">
            Максимка
          </span>
        </Link>

        <nav className="hidden items-center gap-6 sm:flex">
          {navItems.map((item) => {
            const isActive = pathname?.startsWith(item.href);
            return (
              <Link
                key={item.href}
                href={item.href}
                className={`flex items-center gap-1.5 text-sm font-medium transition-colors ${
                  isActive ? item.accent : "text-stone-500 hover:text-stone-900"
                }`}
              >
                <span
                  className={`h-1.5 w-1.5 rounded-full ${isActive ? item.dot : "bg-transparent"}`}
                />
                {item.label}
              </Link>
            );
          })}
        </nav>

        <DropdownMenu>
          <DropdownMenuTrigger className="flex items-center gap-2 rounded-full outline-none ring-offset-2 focus-visible:ring-2 focus-visible:ring-stone-400">
            <Avatar className="h-8 w-8">
              <AvatarFallback className="bg-stone-100 text-xs font-medium text-stone-700">
                МК
              </AvatarFallback>
            </Avatar>
            <ChevronDown className="hidden h-4 w-4 text-stone-400 sm:block" />
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end" className="w-48">
            <DropdownMenuGroup>
              <DropdownMenuLabel>Максимка Киселька</DropdownMenuLabel>
            </DropdownMenuGroup>
            <DropdownMenuSeparator />
            <DropdownMenuGroup>
              <DropdownMenuItem>
                <User className="mr-2 h-4 w-4" />
                Профиль
              </DropdownMenuItem>
              <DropdownMenuItem>
                <Settings className="mr-2 h-4 w-4" />
                Настройки
              </DropdownMenuItem>
            </DropdownMenuGroup>
            <DropdownMenuSeparator />
            <DropdownMenuItem className="text-red-600 focus:text-red-600">
              <LogOut className="mr-2 h-4 w-4" />
              Выйти
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      </div>
    </header>
  );
}
