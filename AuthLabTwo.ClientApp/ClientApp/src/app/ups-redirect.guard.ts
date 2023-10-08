import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class UpsRedirectGuard implements CanActivate {

  constructor(
    private router: Router,
    private auth: AuthService
  ) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    const requestedRoute = state.url;

    const inIframe = window.self !== window.top;
    if (requestedRoute.endsWith('/ups')) {
      if (inIframe) {
        window!.top!.location.href = 'https://localhost:44450/home/ups';
      }

      this.router.navigate([requestedRoute.substring(0, requestedRoute.length - 4)]);
      return false; 
    }

    return true;
  }
}
