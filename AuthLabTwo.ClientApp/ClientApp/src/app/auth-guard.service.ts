import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService {

  constructor(private router: Router, private auth: AuthService) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    return this.auth.getUserInfo().pipe(
      map((x) => {
        let result = !!x;
        let role = x?.role;

        let targetRoute = route.routeConfig?.path as string;

        if (!result) {
          this.auth.authenticateUser(targetRoute);
        }

        return true;
      })
    )
  }
}
