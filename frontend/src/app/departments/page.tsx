import DepartmentsList from "@/components/departments/delartments.list";
import type { Metadata } from "next";

export const metadata: Metadata = {
  title: "Departments",
  description: "Departments page",
};

export default function Departments() {
  return (
    <DepartmentsList/>
  );
}
