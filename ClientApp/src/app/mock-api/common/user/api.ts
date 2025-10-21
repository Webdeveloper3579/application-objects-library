import { Injectable, inject } from '@angular/core';
import { FuseMockApiService } from '@fuse/lib/mock-api';
import { user as userData } from 'app/mock-api/common/user/data';
import { AuthService } from 'app/core/auth/auth.service';
import { assign, cloneDeep } from 'lodash-es';

@Injectable({ providedIn: 'root' })
export class UserMockApi {
    private readonly _authService = inject(AuthService);
    private _user: any = userData;

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
        // @ User - GET
        // -----------------------------------------------------------------------------------------------------
        this._fuseMockApiService
            .onGet('api/common/user')
            .reply(() => {
                // If user is authenticated, try to get user from token
                if (this._authService.accessToken) {
                    try {
                        // Decode JWT token to get user info
                        const token = this._authService.accessToken;
                        const payload = JSON.parse(atob(token.split('.')[1]));
                        
                        // Create user object from token claims
                        const userFromToken = {
                            id: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'],
                            name: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
                            email: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
                            FirstName: payload['FirstName'],
                            Surname: payload['Surname'],
                            IsSiteAdmin: payload['IsSiteAdmin'] === 'True' || payload['IsSiteAdmin'] === true,
                            avatar: 'images/avatars/brian-hughes.jpg', // Default avatar
                            status: 'online'
                        };
                        
                        console.log('=== USER API DEBUG ===');
                        console.log('Token payload:', payload);
                        console.log('User from token:', userFromToken);
                        
                        return [200, cloneDeep(userFromToken)];
                    } catch (error) {
                        console.error('Error decoding token:', error);
                        // Fall back to default user if token is invalid
                        return [200, cloneDeep(this._user)];
                    }
                }
                
                // Return default user if not authenticated
                return [200, cloneDeep(this._user)];
            });

        // -----------------------------------------------------------------------------------------------------
        // @ User - PATCH
        // -----------------------------------------------------------------------------------------------------
        this._fuseMockApiService
            .onPatch('api/common/user')
            .reply(({ request }) => {
                // Get the user mock-api
                const user = cloneDeep(request.body.user);

                // Update the user mock-api
                this._user = assign({}, this._user, user);

                // Return the response
                return [200, cloneDeep(this._user)];
            });
    }
}
