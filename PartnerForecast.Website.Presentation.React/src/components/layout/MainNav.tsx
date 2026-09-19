"use client";

import {
  HomeIcon,
  ClockIcon,
  UsersIcon,
  ListBulletIcon,
} from "@heroicons/react/24/solid";

import Link from "next/link";
import { usePathname } from "next/navigation";
import type { ComponentType, SVGProps } from "react";

type NavigationItem = {
  name: string;
  href: string;
  icon: ComponentType<SVGProps<SVGSVGElement>>;
};

export const navigation: NavigationItem[] = [
  { name: "Home", href: "/", icon: HomeIcon },
  { name: "Hours", href: "/hours", icon: ClockIcon },
  { name: "NAO Hours", href: "/nao-hours", icon: ClockIcon },
  { name: "NAO Users", href: "/nao-users", icon: UsersIcon },
  { name: "Task Codes", href: "/task-codes", icon: ListBulletIcon },
];

type MainNavProps = {
  readonly isCollapsed?: boolean;
  readonly className?: string;
  readonly variant?: "sidebar" | "links";
};

export default function MainNav({
  isCollapsed = false,
  className = "",
  variant = "sidebar",
}: MainNavProps) {
  const pathname = usePathname();
  const isSidebarVariant = variant === "sidebar";

  const getLinkClass = (isActive: boolean) => {
    return isActive ? "text-white" : "text-white/70";
  };

  const getActiveLinkClass = (isActive: boolean) => {
    return isActive ? "text-green-700" : "text-gray-500";
  };

  const getLinkWidth = (isCollapsed: boolean) => {
    return isCollapsed ? "w-6 h-6" : "w-5 h-5 mr-3";
  };

  const getNavClassName = (isSidebarVariant: boolean, className: string) => {
    if (isSidebarVariant) {
      return `flex-1 px-2 py-4 space-y-1 ${className}`.trim();
    }

    return `flex flex-col items-start gap-6 ${className}`.trim();
  };

  const getSidebarItemClassName = (isActive: boolean) => {
    if (isActive) {
      return "bg-green-600 text-white";
    }

    return "text-white/70 hover:bg-[#0F2A4A] hover:text-white";
  };

  const getLinksItemClassName = (isActive: boolean) => {
    if (isActive) {
      return "text-green-700";
    }

    return "text-gray-700 hover:text-gray-900";
  };

  const getItemClassName = (isSidebarVariant: boolean, isActive: boolean) => {
    if (isSidebarVariant) {
      return `
        flex items-center px-3 py-2 rounded-md text-sm font-medium
        ${getSidebarItemClassName(isActive)}
      `;
    }

    return `
      inline-flex items-center text-base font-medium
      ${getLinksItemClassName(isActive)}
    `;
  };

  const getIconClassName = (
    isSidebarVariant: boolean,
    isCollapsed: boolean,
    isActive: boolean,
  ) => {
    if (isSidebarVariant) {
      return `
        ${getLinkWidth(isCollapsed)}
        ${getLinkClass(isActive)}
      `;
    }

    return `mr-2 h-5 w-5 ${getActiveLinkClass(isActive)}`;
  };

  return (
    <nav className={getNavClassName(isSidebarVariant, className)}>
      {navigation.map((item) => {
        const isActive =
          pathname.replace(/\/$/, "") === item.href || pathname === item.href;
        return (
          <Link
            key={item.name}
            href={item.href}
            className={getItemClassName(isSidebarVariant, isActive)}
          >
            <item.icon
              className={getIconClassName(
                isSidebarVariant,
                isCollapsed,
                isActive,
              )}
            />
            {(!isCollapsed || !isSidebarVariant) && <span>{item.name}</span>}
          </Link>
        );
      })}
    </nav>
  );
}
