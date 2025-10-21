import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-customer',
    template: `
        <div class="flex flex-col items-center justify-center min-h-[400px] text-center">
            <div class="max-w-md mx-auto">
                <div class="mb-6">
                    <div class="w-16 h-16 mx-auto mb-4 bg-green-100 dark:bg-green-900 rounded-full flex items-center justify-center">
                        <svg class="w-8 h-8 text-green-600 dark:text-green-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"></path>
                        </svg>
                    </div>
                    <h2 class="text-2xl font-bold text-gray-900 dark:text-white mb-2">Customer Portal</h2>
                    <p class="text-gray-600 dark:text-gray-400">Access your objects and fill out forms</p>
                </div>
                
                <div class="space-y-4">
                    <p class="text-sm text-gray-500 dark:text-gray-400">
                        No objects available yet. Objects will appear in the navigation menu once they are created by administrators.
                    </p>
                </div>
            </div>
        </div>
    `,
    imports: [CommonModule],
    standalone: true
})
export class CustomerComponent {
}
