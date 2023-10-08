import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

export class UserProfile {
  name: string;
  role: string;
  id: string;

  constructor() {
    this.name = "";
    this.role = "";
    this.id = "";
  }
}

@Injectable({
  providedIn: 'root'
})

export class AuthService {

  apiBaseUrl: string = "";
  baseUrl: string = "";
  user = new BehaviorSubject<UserProfile | null>(null);

  constructor(
    private http: HttpClient,
    @Inject('API_BASE_URL') apiBaseUrl: string,
    @Inject('BASE_URL') baseUrl: string,
  ) {
    this.apiBaseUrl = apiBaseUrl;
    this.baseUrl = baseUrl;
  }

  public authenticateUser(redirect: string): void {
    const redirectUrl = 'https://localhost:5002/' + `accounts/login?redirect=${redirect}`;
    window.location.replace(redirectUrl) 
  }

  public getUserInfo(): Observable<UserProfile> {
    const userInfo = this.http.get<UserProfile>(this.apiBaseUrl + "accounts/user-info", { withCredentials: true });
    console.log(userInfo);
    return userInfo;
  }
}
