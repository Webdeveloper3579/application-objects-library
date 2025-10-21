import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-admin',
    template: `
        <div class="flex flex-col items-center justify-center min-h-[400px] text-center">
            <div class="max-w-md mx-auto">
                <div class="mb-6">
                    <div class="w-16 h-16 mx-auto mb-4 bg-blue-100 dark:bg-blue-900 rounded-full flex items-center justify-center">
                        <svg class="w-8 h-8 text-blue-600 dark:text-blue-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10"></path>
                        </svg>
                    </div>
                    <h2 class="text-2xl font-bold text-gray-900 dark:text-white mb-2">Admin Dashboard</h2>
                    <p class="text-gray-600 dark:text-gray-400">Manage your objects and fields</p>
                </div>
                
                <div class="space-y-4">
                    <p class="text-sm text-gray-500 dark:text-gray-400">
                        No objects available yet. Objects will appear in the navigation menu once they are created.
                    </p>
                    
                    <button 
                        type="button"
                        class="px-4 py-2 text-sm font-medium text-white bg-blue-600 border border-transparent rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500"
                        (click)="openAddObjectDialog()"
                    >
                        <svg class="w-4 h-4 mr-2 inline" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6"></path>
                        </svg>
                        Add First Object
                    </button>
                </div>
            </div>
        </div>
    `,
    imports: [CommonModule],
    standalone: true
})
export class AdminComponent {
    openAddObjectDialog(): void {
        window.dispatchEvent(new CustomEvent('openAddObjectDialog'));
    }
}
