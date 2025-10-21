import { Routes } from '@angular/router';
import { CustomerObjectViewComponent } from 'app/modules/customer/object/customer-object-view.component';

export default [
    {
        path     : ':id',
        component: CustomerObjectViewComponent,
    },
] as Routes;
