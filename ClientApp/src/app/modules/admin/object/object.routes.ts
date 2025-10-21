import { Routes } from '@angular/router';
import { ObjectComponent } from 'app/modules/admin/object/object.component';

export default [
    {
        path     : ':id',
        component: ObjectComponent,
    },
] as Routes;
