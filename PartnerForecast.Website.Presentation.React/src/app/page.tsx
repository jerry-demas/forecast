'use client';
import MainNav from '@/components/layout/MainNav';

export default function LandingPage() {
    return (        
    <div className="max-w-7xl mx-auto">
        <div className="mb-6">
            <h2 className="text-2xl font-bold text-gray-900">Dashboard</h2>
        </div>

        <div className="bg-white rounded-lg shadow-md overflow-hidden">
            <div className="px-6 py-4 border-b border-gray-200">
                <div className="flex items-center justify-between">
                <div className="flex items-center space-x-2">
                    <div className="w-full rounded-md">
                    <MainNav variant="links" />
                    </div>
                </div>
                </div>
            </div>
        </div>
    </div>
    );
}