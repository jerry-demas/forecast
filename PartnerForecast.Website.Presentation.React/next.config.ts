import type { NextConfig } from "next";

const appVersion =
  process.env.NEXT_PUBLIC_APP_VERSION ??
  process.env.npm_package_version ??
  '0.0.0';

const nextConfig: NextConfig = {
  output: 'export',
  distDir: 'out/wwwroot',
  images: { unoptimized: true },
  trailingSlash: true,
  basePath: '',
  assetPrefix: '',
  env: {
    NEXT_PUBLIC_APP_VERSION: appVersion,
  },
};

export default nextConfig;
