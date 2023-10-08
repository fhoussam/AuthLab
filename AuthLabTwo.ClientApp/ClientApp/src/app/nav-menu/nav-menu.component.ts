import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { AuthService, UserProfile } from '../auth.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css'],
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  showLogin = false;
  loginUrl = "https://localhost:5002/accounts/login?redirect=home";
  user: UserProfile | null = null;

  urlSafe: SafeResourceUrl = {};
  constructor(private sanitizer: DomSanitizer, private auth: AuthService) {
    this.auth.user.subscribe(x => {
      console.log("user profile from nav", x?.name);
      this.user = x;
    });
  }

  ngOnInit() {
    this.urlSafe = this.sanitizer.bypassSecurityTrustResourceUrl(this.loginUrl);
  }

  collapse() {
    this.isExpanded = false;
  }

  toggleLoginModal(showLogin: boolean) {
    this.showLogin = showLogin;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
