import Footer from "@/components/layout/Footer";
import Header from "@/components/layout/Header";
import Sidebar from "@/components/layout/Sidebar";
import { Toaster } from "@/components/ui/sonner";
import { UserProvider } from "@/contexts/UserContexts";
import type { Metadata } from "next";
import { Inter } from "next/font/google";
import "./globals.css";

const inter = Inter({ subsets: ["latin"] });
const appName: string = process.env.NEXT_PUBLIC_APP_NAME ?? '';
const appDescription: string = process.env.NEXT_PUBLIC_APP_DESCRIPTION ?? '';
export const metadata: Metadata = {
  title: appName,
  description: appDescription,
  applicationName: appName,
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body className={inter.className}>
        <UserProvider>
          <div className="flex h-screen">
            <Sidebar />
            <div className="flex-1 flex flex-col">
              <Header />
              <main className="flex-1 overflow-y-auto bg-gray-50 p-8">
                {children}
              </main>
              <Toaster position="bottom-right" richColors />
              <Footer />
            </div>
          </div>
        </UserProvider>
      </body>
    </html>
  );
}
