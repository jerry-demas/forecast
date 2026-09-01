'use client'

import { useState } from 'react'
import {
    ChevronDoubleLeftIcon, ChevronDoubleRightIcon, CalendarDaysIcon 
} from '@heroicons/react/24/outline'
import MainNav from './MainNav'

export default function Sidebar() {
  const [isCollapsed, setIsCollapsed] = useState(false)

  return (
    <div className={`bg-[#1B365D] border-r border-[#1B365D] ${isCollapsed ? 'w-20' : 'w-80'} transition-all duration-300 ease-in-out`}>
      <div className="flex flex-col h-full">
        <div className="flex items-center justify-between h-16 px-4 border-b border-[#1B365D]">
          {!isCollapsed && (
            <div className="flex items-center">
              <CalendarDaysIcon className="w-6 h-6 text-white" />
                          <span className="text-xl font-semibold text-white ml-2">{process.env.NEXT_PUBLIC_APP_NAME}</span>
            </div>
          )}
          <button
            onClick={() => setIsCollapsed(!isCollapsed)}
            className="p-2 rounded-md hover:bg-[#0F2A4A]"
                  >
            {isCollapsed ? (<ChevronDoubleRightIcon className="w-6 h-6 text-white/70 hover:text-white" />) : <ChevronDoubleLeftIcon className="w-6 h-6 text-white/70 hover:text-white" /> }
          </button>
        </div>

        <MainNav isCollapsed={isCollapsed} />
        
      </div>
    </div>
  )
}

 