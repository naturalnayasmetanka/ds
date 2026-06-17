import Link from "next/link";

const year = new Date().getFullYear();

// Серверный компонент: контент статический, "use client" не нужен.
export default function Footer() {
  return (
    <footer className="border-t border-stone-200 bg-white">
      <div className="mx-auto flex max-w-5xl flex-col gap-3 px-6 py-6 text-sm text-stone-500 sm:flex-row sm:items-center sm:justify-between">
        <p>© {year} Максимка Panel. Внутренний инструмент компании.</p>

        <div className="flex items-center gap-3">
          <span className="flex items-center gap-1.5">
            <span className="h-1.5 w-1.5 rounded-full bg-emerald-500" />
            Все системы работают
          </span>
          <span className="text-stone-300">·</span>
          <Link href="/admin/docs" className="hover:text-stone-900">
            Документация
          </Link>
          <span className="text-stone-300">·</span>
          <Link href="/admin/support" className="hover:text-stone-900">
            Поддержка
          </Link>
        </div>
      </div>
    </footer>
  );
}
