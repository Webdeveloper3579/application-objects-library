import { Injectable } from '@angular/core';
import { Resolve, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class AdminRedirectResolver implements Resolve<any> {
    constructor(
        private http: HttpClient,
        private router: Router
    ) {}

    resolve(): Observable<any> {
        return this.http.get<any[]>('/api/Object').pipe(
            map((objects) => {
                if (objects && objects.length > 0) {
                    // Redirect to the first object
                    this.router.navigate([`/admin/object/${objects[0].Id}`]);
                } else {
                    // If no objects exist, stay on admin page (will show empty state)
                    this.router.navigate(['/admin']);
                }
                return objects;
            }),
            catchError(() => {
                // On error, redirect to admin page
                this.router.navigate(['/admin']);
                return of([]);
            })
        );
    }
}
