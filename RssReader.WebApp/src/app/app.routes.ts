import { Routes } from '@angular/router';
import { SignupComponent } from './pages/signup/signup.component';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { EmailVerificationComponent } from './pages/email-verification/email-verification.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { isSignedUpGuard } from './services/guards/is-signed-up.guard';
import { isAnonymousGuard } from './services/guards/is-anonymous.guard';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { isLoggedInGuard } from './services/guards/is-logged-in.guard';

export const routes: Routes = [
    { path: '', title: 'RssReader', component: HomeComponent},
    { path: 'sign-up', title: 'Sign up', component: SignupComponent, canActivate: [isAnonymousGuard]},
    { path: 'login', title: 'Log in', component: LoginComponent, canActivate: [isAnonymousGuard]},
    { path: 'email-verification', title: 'Verify your email', component: EmailVerificationComponent, canActivate: [isSignedUpGuard]},
    { path: 'dashboard', title: 'Dashboard', component: DashboardComponent, canActivate: [isLoggedInGuard]},
    { path: '**', title: 'Not found :(', component: NotFoundComponent},
];
