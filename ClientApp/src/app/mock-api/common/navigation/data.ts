/* eslint-disable */
import { FuseNavigationItem } from '@fuse/components/navigation';
import { cloneDeep } from 'lodash-es';

// Admin navigation (for admin users)
export const adminNavigation: FuseNavigationItem[] = [
    {
        id   : 'add-object',
        title: 'Add Object',
        type : 'basic',
        icon : 'heroicons_outline:plus-circle',
        function: (item: any) => {
            // Dispatch custom event to open dialog
            window.dispatchEvent(new CustomEvent('openAddObjectDialog'));
        },
        classes: {
            wrapper: 'px-3 py-1.5 text-xs font-medium text-white bg-blue-600 border border-transparent rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 flex items-center space-x-1 mx-2 my-1'
        }
    }
];

// Customer navigation (for regular users)
export const customerNavigation: FuseNavigationItem[] = [
    {
        id   : 'customer',
        title: 'Customer',
        type : 'basic',
        icon : 'heroicons_outline:chart-pie',
        link : '/customer/customer'
    },
    {
        id   : 'city',
        title: 'City',
        type : 'basic',
        icon : 'heroicons_outline:map-pin',
        link : '/customer/city'
    }
];

// Default navigation (admin navigation for backward compatibility)
export const defaultNavigation: FuseNavigationItem[] = adminNavigation;

// Since all navigation items are basic (no children), we can reuse the same structure
export const compactNavigation: FuseNavigationItem[] = cloneDeep(defaultNavigation);
export const futuristicNavigation: FuseNavigationItem[] = cloneDeep(defaultNavigation);
export const horizontalNavigation: FuseNavigationItem[] = cloneDeep(defaultNavigation);
