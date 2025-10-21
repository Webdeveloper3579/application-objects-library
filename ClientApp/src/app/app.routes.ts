import { Route } from '@angular/router';
import { initialDataResolver } from 'app/app.resolvers';
import { AuthGuard } from 'app/core/auth/guards/auth.guard';
import { NoAuthGuard } from 'app/core/auth/guards/noAuth.guard';
import { LayoutComponent } from 'app/layout/layout.component';
import { AdminComponent } from 'app/modules/admin/admin.component';
import { CustomerComponent } from 'app/modules/customer/customer.component';
import { AdminRedirectResolver } from 'app/modules/admin/admin-redirect.resolver';
import { CustomerRedirectResolver } from 'app/modules/customer/customer-redirect.resolver';

// @formatter:off
/* eslint-disable max-len */
/* eslint-disable @typescript-eslint/explicit-function-return-type */
export const appRoutes: Route[] = [

    // Redirect empty path to '/admin' (will be handled by role-based routing)
    {path: '', pathMatch : 'full', redirectTo: 'admin'},

    // Redirect signed-in user - this will be handled by the sign-in component based on user role
    //
    // After the user signs in, the sign-in page will redirect the user to the 'signed-in-redirect'
    // path. The sign-in component will determine the correct destination based on user role.
    // Note: This redirect is removed to allow the sign-in component to handle role-based routing.
    // {path: 'signed-in-redirect', pathMatch : 'full', redirectTo: 'admin/customer'},

    // Auth routes for guests
    {
        path: '',
        canActivate: [NoAuthGuard],
        canActivateChild: [NoAuthGuard],
        component: LayoutComponent,
        data: {
            layout: 'empty'
        },
        children: [
            {path: 'forgot-password', loadChildren: () => import('app/modules/auth/forgot-password/forgot-password.routes')},
            {path: 'reset-password', loadChildren: () => import('app/modules/auth/reset-password/reset-password.routes')},
            {path: 'sign-in', loadChildren: () => import('app/modules/auth/sign-in/sign-in.routes')},
            {path: 'sign-up', loadChildren: () => import('app/modules/auth/sign-up/sign-up.routes')}
        ]
    },

    // Auth routes for authenticated users
    {
        path: '',
        canActivate: [AuthGuard],
        canActivateChild: [AuthGuard],
        component: LayoutComponent,
        data: {
            layout: 'empty'
        },
        children: [
            {path: 'sign-out', loadChildren: () => import('app/modules/auth/sign-out/sign-out.routes')},
            {path: 'unlock-session', loadChildren: () => import('app/modules/auth/unlock-session/unlock-session.routes')}
        ]
    },

    // Landing routes
    {
        path: '',
        component: LayoutComponent,
        data: {
            layout: 'empty'
        },
        children: [
            {path: 'home', loadChildren: () => import('app/modules/landing/home/home.routes')},
        ]
    },

    // Admin routes
    {
        path: 'admin',
        canActivate: [AuthGuard],
        canActivateChild: [AuthGuard],
        component: LayoutComponent,
        resolve: {
            initialData: initialDataResolver
        },
        children: [
            {path: '', component: AdminComponent, resolve: { objects: AdminRedirectResolver }},
            {path: 'object', loadChildren: () => import('app/modules/admin/object/object.routes')},
        ]
    },

       // Customer routes (for regular users)
       {
           path: 'customer',
           canActivate: [AuthGuard],
           canActivateChild: [AuthGuard],
           component: LayoutComponent,
           resolve: {
               initialData: initialDataResolver
           },
           children: [
               {path: '', component: CustomerComponent, resolve: { objects: CustomerRedirectResolver }},
               {path: 'object', loadChildren: () => import('app/modules/customer/object/customer-object-view.routes')},
           ]
       }
];
