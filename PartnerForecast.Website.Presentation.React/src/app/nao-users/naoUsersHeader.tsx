"use client";
import {UsersIcon} from '@heroicons/react/24/solid'
export default function NaoUsersHeader() {
    return (
        <div className="flex items-center justify-between">        
            <div className="flex items-center justify-between">
                <div className="flex items-center gap-2">
                    <UsersIcon className="h-6 w-6 text-gray-600" />
                    <h1 className="text-2xl font-semibold text-gray-900">
                        NAO Users
                    </h1>                    
                </div>
            </div>
        </div>
    );
}