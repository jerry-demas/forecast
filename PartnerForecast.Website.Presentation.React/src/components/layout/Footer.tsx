'use client'

export default function Footer() {
    return (
        <footer className="bg-white border-t border-gray-200 px-8 py-4">
            <div className="flex items-center justify-between text-sm text-gray-500">
                <div>
                    @{process.env.NEXT_PUBLIC_APP_COPYRIGHT ?? ''}
                </div>
                <div className="flex items-center space-x-4">              
                    <span>                         
                        {process.env.NEXT_PUBLIC_APP_VERSION ?? ''}
                    </span>
                    <span>                         
                        Last Update: {process.env.NEXT_PUBLIC_LAST_BUILD_DATE ?? ''}
                    </span>
                </div>
            </div>
        </footer>
    )
}