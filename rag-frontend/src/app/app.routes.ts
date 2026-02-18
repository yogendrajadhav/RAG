import { Routes } from '@angular/router';

export const routes: Routes = [
    { path: '/upload', loadComponent: () => import('./components/file-upload/file-upload').then(m => m.FileUpload)},
    { path: '/query', loadComponent: () => import('./components/query-interface/query-interface').then(m => m.QueryInterface)},
    
];

