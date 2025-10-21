import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Navigation } from 'app/core/navigation/navigation.types';
import { Observable, ReplaySubject, tap, forkJoin, map } from 'rxjs';
import { FuseNavigationItem } from '@fuse/components/navigation';

export interface Object {
    Id: number;
    ObjectName: string;
    ObjectEnum: string;
    CreatedDate: string;
    ModifiedDate?: string;
}

@Injectable({ providedIn: 'root' })
export class NavigationService {
    private _httpClient = inject(HttpClient);
    private _navigation: ReplaySubject<Navigation> =
        new ReplaySubject<Navigation>(1);

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Getter for navigation
     */
    get navigation$(): Observable<Navigation> {
        return this._navigation.asObservable();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get navigation data for admin
     */
    get(): Observable<Navigation> {
        return this._httpClient.get<Object[]>('/api/Object').pipe(
            map((objects) => {
                // Create dynamic navigation items from objects for admin
                const adminObjectNavigationItems: FuseNavigationItem[] = objects.map(obj => ({
                    id: `admin-object-${obj.Id}`,
                    title: obj.ObjectName,
                    type: 'basic',
                    icon: 'heroicons_outline:cube',
                    link: `/admin/object/${obj.Id}`,
                    badge: {
                        title: this.getObjectEnumInitial(obj.ObjectEnum),
                        classes: this.getObjectEnumBadgeClasses(obj.ObjectEnum)
                    },
                    meta: {
                        objectEnum: obj.ObjectEnum
                    }
                }));

                // Create dynamic navigation items from objects for customer
                const customerObjectNavigationItems: FuseNavigationItem[] = objects.map(obj => ({
                    id: `customer-object-${obj.Id}`,
                    title: obj.ObjectName,
                    type: 'basic',
                    icon: 'heroicons_outline:cube',
                    link: `/customer/object/${obj.Id}`,
                    badge: {
                        title: this.getObjectEnumInitial(obj.ObjectEnum),
                        classes: this.getObjectEnumBadgeClasses(obj.ObjectEnum)
                    },
                    meta: {
                        objectEnum: obj.ObjectEnum
                    }
                }));

                // Create new admin navigation with only dynamic objects and Add Object button
                const dynamicAdminNavigation: FuseNavigationItem[] = [
                    ...adminObjectNavigationItems,
                    {
                        id: 'add-object',
                        title: 'Add Object',
                        type: 'basic',
                        icon: 'heroicons_outline:plus-circle',
                        function: (item: any) => {
                            window.dispatchEvent(new CustomEvent('openAddObjectDialog'));
                        },
                        classes: {
                            wrapper: 'px-3 py-1.5 text-xs font-medium text-white bg-blue-600 border border-transparent rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 flex items-center space-x-1 mx-2 my-1'
                        }
                    }
                ];

                // Create navigation structure with dynamic admin navigation
                const navigation: Navigation = {
                    compact: dynamicAdminNavigation,
                    default: dynamicAdminNavigation,
                    futuristic: dynamicAdminNavigation,
                    horizontal: dynamicAdminNavigation
                };

                return navigation;
            }),
            tap((navigation) => {
                this._navigation.next(navigation);
            })
        );
    }

    /**
     * Get navigation data for customer
     */
    getCustomerNavigation(): Observable<Navigation> {
        return this._httpClient.get<Object[]>('/api/Object').pipe(
            map((objects) => {
                // Create dynamic navigation items from objects for customer
                const customerObjectNavigationItems: FuseNavigationItem[] = objects.map(obj => ({
                    id: `customer-object-${obj.Id}`,
                    title: obj.ObjectName,
                    type: 'basic',
                    icon: 'heroicons_outline:cube',
                    link: `/customer/object/${obj.Id}`,
                    badge: {
                        title: this.getObjectEnumInitial(obj.ObjectEnum),
                        classes: this.getObjectEnumBadgeClasses(obj.ObjectEnum)
                    },
                    meta: {
                        objectEnum: obj.ObjectEnum
                    }
                }));

                // Create customer navigation with only dynamic objects
                const customerNavigation: FuseNavigationItem[] = [
                    ...customerObjectNavigationItems
                ];

                // Create navigation structure with customer navigation
                const navigation: Navigation = {
                    compact: customerNavigation,
                    default: customerNavigation,
                    futuristic: customerNavigation,
                    horizontal: customerNavigation
                };

                return navigation;
            })
        );
    }

    /**
     * Get ObjectEnum initial
     */
    private getObjectEnumInitial(objectEnum: string): string
    {
        if (!objectEnum || objectEnum.length === 0) return '?';
        return objectEnum.charAt(0).toUpperCase();
    }

    /**
     * Get ObjectEnum badge classes
     */
    private getObjectEnumBadgeClasses(objectEnum: string): string
    {
        const baseClasses = 'rounded-full px-2 py-1 text-xs font-medium';
        switch (objectEnum.toLowerCase()) {
            case 'standard': return `${baseClasses} bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-200`;
            case 'custom': return `${baseClasses} bg-purple-100 text-purple-800 dark:bg-purple-900 dark:text-purple-200`;
            default: return `${baseClasses} bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-200`;
        }
    }
}
