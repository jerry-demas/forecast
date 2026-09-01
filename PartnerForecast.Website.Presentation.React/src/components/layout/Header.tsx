'use client'

import {
    BellIcon//, UserIcon
} from '@heroicons/react/24/outline'
import { useUser } from '@/contexts/UserContexts'

export default function Header() {
  const user = useUser()

  return (
    <header className="bg-white border-b border-border">
      <div className="h-8 px-4 flex items-center justify-between">
        <div className="flex items-center space-x-4">
        </div>
        <div className="flex items-center space-x-4">
           <button className="text-text-light hover:text-text transition-colors">
             <BellIcon className="w-6 h-6" />
           </button>
           <button className="text-text-light hover:text-text transition-colors">
             {/* <UserIcon className="w-6 h-6" /> */}
             Hello, {user?.employeeName ?? 'User'}
           </button>
        </div>
      </div>
    </header>
  )
} 