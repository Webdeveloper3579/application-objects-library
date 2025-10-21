import { Injectable, inject } from '@angular/core';
import { FuseNavigationItem } from '@fuse/components/navigation';
import { FuseMockApiService } from '@fuse/lib/mock-api';
import {
    compactNavigation,
    defaultNavigation,
    futuristicNavigation,
    horizontalNavigation,
    adminNavigation,
    customerNavigation,
} from 'app/mock-api/common/navigation/data';
import { UserService } from 'app/core/user/user.service';
import { cloneDeep } from 'lodash-es';

@Injectable({ providedIn: 'root' })
export class NavigationMockApi {
    private readonly _userService = inject(UserService);
    private readonly _compactNavigation: FuseNavigationItem[] =
        compactNavigation;
    private readonly _defaultNavigation: FuseNavigationItem[] =
        defaultNavigation;
    private readonly _futuristicNavigation: FuseNavigationItem[] =
        futuristicNavigation;
    private readonly _horizontalNavigation: FuseNavigationItem[] =
        horizontalNavigation;
    private readonly _adminNavigation: FuseNavigationItem[] =
        adminNavigation;
    private readonly _customerNavigation: FuseNavigationItem[] =
        customerNavigation;

    /**
     * Constructor
     */
    constructor(private _fuseMockApiService: FuseMockApiService) {
        // Register Mock API handlers
        this.registerHandlers();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Register Mock API handlers
     */
    registerHandlers(): void {
        // -----------------------------------------------------------------------------------------------------
        // @ Navigation - GET
        // -----------------------------------------------------------------------------------------------------
        this._fuseMockApiService.onGet('api/common/navigation').reply(() => {
            // Get current user to determine role-based navigation
            const user = this._userService.user;
            const isAdmin = user && user.IsSiteAdmin === true;
            
            console.log('=== NAVIGATION DEBUG ===');
            console.log('User:', user);
            console.log('IsAdmin:', isAdmin);
            
            // If user is not loaded yet, return default navigation (admin navigation)
            // This prevents hard refresh issues when user data is not yet available
            const roleBasedNavigation = user ? 
                (isAdmin ? this._adminNavigation : this._customerNavigation) : 
                this._adminNavigation;
            
            console.log('Role-based navigation:', roleBasedNavigation);

            // Fill compact navigation children using the role-based navigation
            this._compactNavigation.forEach((compactNavItem) => {
                roleBasedNavigation.forEach((roleNavItem) => {
                    if (roleNavItem.id === compactNavItem.id) {
                        compactNavItem.children = cloneDeep(
                            roleNavItem.children
                        );
                    }
                });
            });

            // Fill futuristic navigation children using the role-based navigation
            this._futuristicNavigation.forEach((futuristicNavItem) => {
                roleBasedNavigation.forEach((roleNavItem) => {
                    if (roleNavItem.id === futuristicNavItem.id) {
                        futuristicNavItem.children = cloneDeep(
                            roleNavItem.children
                        );
                    }
                });
            });

            // Fill horizontal navigation children using the role-based navigation
            this._horizontalNavigation.forEach((horizontalNavItem) => {
                roleBasedNavigation.forEach((roleNavItem) => {
                    if (horizontalNavItem.id === roleNavItem.id) {
                        horizontalNavItem.children = cloneDeep(
                            roleNavItem.children
                        );
                    }
                });
            });

            // Return the response with role-based navigation
            return [
                200,
                {
                    compact: cloneDeep(roleBasedNavigation),
                    default: cloneDeep(roleBasedNavigation),
                    futuristic: cloneDeep(roleBasedNavigation),
                    horizontal: cloneDeep(roleBasedNavigation),
                },
            ];
        });
    }
}
