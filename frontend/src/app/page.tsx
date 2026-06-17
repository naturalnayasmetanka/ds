import Locations from "./locations/page";

import type { Metadata } from "next";

export const metadata: Metadata = {
  title: "Home",
  description: "Home page",
};

export default function Home() {
  return <Locations />;
}
