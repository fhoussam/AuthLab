import { CookieService } from 'ngx-cookie-service';
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
    private auth: AuthService,
    private cookieService: CookieService
  ) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    const requestedRoute = state.url;

    if (requestedRoute.endsWith('/ups')) {
      let jsonUserProfile = this.cookieService.get('userProfile');
      const userProfile = JSON.parse(jsonUserProfile);
      console.log(userProfile);
      this.auth.user.next(userProfile);
      this.router.navigate([requestedRoute.substring(0, requestedRoute.length - 4)]);
      return false; 
    }

    return true;
  }
}
