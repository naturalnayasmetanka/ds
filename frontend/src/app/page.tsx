import Link from "next/link";
import { ArrowRight, Briefcase, Building2, MapPin } from "lucide-react";
import {
  Card,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";

import type { Metadata } from "next";
import { routes } from "@/shared/routes";

export const metadata: Metadata = {
  title: "Home",
  description: "Home page",
};

const sections = [
  {
    href: routes.departments,
    title: "Отделы",
    description: "Структура компании и список подразделений",
    icon: Building2,
  },
  {
    href: routes.locations,
    title: "Локации",
    description: "Офисы и точки присутствия компании",
    icon: MapPin,
  },
  {
    href: routes.positions,
    title: "Должности",
    description: "Справочник должностей и грейдов",
    icon: Briefcase,
  },
];

export default function AdminPage() {
  return (
    <div className="bg-slate-50 px-6 py-12">
      <div className="mx-auto max-w-5xl">
        <header className="mb-10">
          <p className="mb-2 text-sm font-medium text-indigo-600">
            Панель управления
          </p>
          <h1 className="text-3xl font-semibold tracking-tight text-slate-900">
            Admin panel
          </h1>
          <p className="mt-2 text-slate-500">
            Выберите раздел, чтобы начать работу со справочниками
          </p>
        </header>

        <h2 className="mb-4 text-sm font-medium uppercase tracking-wide text-slate-400">
          Список разделов
        </h2>

        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
          {sections.map((section) => {
            const Icon = section.icon;
            return (
              <Link key={section.href} href={section.href} className="group">
                <Card className="h-full transition-all hover:border-indigo-300 hover:shadow-md">
                  <CardHeader>
                    <div className="mb-3 inline-flex h-10 w-10 items-center justify-center rounded-lg bg-indigo-50 text-indigo-600">
                      <Icon className="h-5 w-5" />
                    </div>
                    <CardTitle className="flex items-center justify-between">
                      {section.title}
                      <ArrowRight className="h-4 w-4 text-slate-300 transition-transform group-hover:translate-x-1 group-hover:text-indigo-600" />
                    </CardTitle>
                    <CardDescription>{section.description}</CardDescription>
                  </CardHeader>
                </Card>
              </Link>
            );
          })}
        </div>
      </div>
    </div>
  );
}
