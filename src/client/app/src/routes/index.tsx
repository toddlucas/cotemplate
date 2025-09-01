import { Routes, Route } from 'react-router-dom';

import Layout from '../layouts/Layout';
import AuthProtected from "./AuthProtected";

import App from '../App';

// Auth views
import Login from "$/features/auth/views/Login";
import Logout from "$/features/auth/views/Logout";
import Register from "$/features/auth/views/Register";
import ForgotPassword from "$/features/auth/views/ForgotPassword";
import AccessDenied from "$/features/auth/views/AccessDenied";
import ConfirmEmail from "$/features/auth/views/ConfirmEmail";
import ConfirmEmailChange from "$/features/auth/views/ConfirmEmailChange";
import ExternalLogin from "$/features/auth/views/ExternalLogin";
import ForgotPasswordConfirmation from "$/features/auth/views/ForgotPasswordConfirmation";
import Lockout from "$/features/auth/views/Lockout";
import LoginWith2fa from "$/features/auth/views/LoginWith2fa";
import LoginWithRecoveryCode from "$/features/auth/views/LoginWithRecoveryCode";
import RegisterConfirmation from "$/features/auth/views/RegisterConfirmation";
import ResendEmailConfirmation from "$/features/auth/views/ResendEmailConfirmation";
import ResetPassword from "$/features/auth/views/ResetPassword";
import ResetPasswordConfirmation from "$/features/auth/views/ResetPasswordConfirmation";

// Account management views
import ChangePassword from "$/features/auth/views/account/ChangePassword";
import DeletePersonalData from "$/features/auth/views/account/DeletePersonalData";
import Disable2fa from "$/features/auth/views/account/Disable2fa";
import DownloadPersonalData from "$/features/auth/views/account/DownloadPersonalData";
import Email from "$/features/auth/views/account/Email";
import EnableAuthenticator from "$/features/auth/views/account/EnableAuthenticator";
import ExternalLogins from "$/features/auth/views/account/ExternalLogins";
import GenerateRecoveryCodes from "$/features/auth/views/account/GenerateRecoveryCodes";
import PersonalData from "$/features/auth/views/account/PersonalData";
import ResetAuthenticator from "$/features/auth/views/account/ResetAuthenticator";
import SetPassword from "$/features/auth/views/account/SetPassword";
import ShowRecoveryCodes from "$/features/auth/views/account/ShowRecoveryCodes";
import TwoFactorAuthentication from "$/features/auth/views/account/TwoFactorAuthentication";

// Feature test views
import AuthViewsIndex from "$/features/auth/views";
import AuthTestPage from "$/features/auth/views/AuthTestPage";

const PlatformRoutes = () => (
  <Routes>
    {/* Public routes */}
    <Route element={<Layout />}>
      <Route path="/" element={<App />} />

      {/* Auth routes */}
      <Route path="signin" element={<Login />} />
      <Route path="signout" element={<Logout />} />
      <Route path="signup" element={<Register />} />
      <Route path="forgot-password" element={<ForgotPassword />} />
      <Route path="access-denied" element={<AccessDenied />} />
      <Route path="confirm-email" element={<ConfirmEmail />} />
      <Route path="confirm-email-change" element={<ConfirmEmailChange />} />
      <Route path="external-login" element={<ExternalLogin />} />
      <Route path="forgot-password-confirmation" element={<ForgotPasswordConfirmation />} />
      <Route path="lockout" element={<Lockout />} />
      <Route path="login-with-2fa" element={<LoginWith2fa />} />
      <Route path="login-with-recovery-code" element={<LoginWithRecoveryCode />} />
      <Route path="register-confirmation" element={<RegisterConfirmation />} />
      <Route path="resend-email-confirmation" element={<ResendEmailConfirmation />} />
      <Route path="reset-password" element={<ResetPassword />} />
      <Route path="reset-password-confirmation" element={<ResetPasswordConfirmation />} />

      {/* Test routes */}
      <Route path="auth-test" element={<AuthViewsIndex />} />
      <Route path="auth-system-test" element={<AuthTestPage />} />
    </Route>

    {/* Routes that require authentication */}
    <Route element={<AuthProtected />}>
      <Route element={<Layout />}>
        <Route path="account/change-password" element={<ChangePassword />} />
        <Route path="account/delete-personal-data" element={<DeletePersonalData />} />
        <Route path="account/disable-2fa" element={<Disable2fa />} />
        <Route path="account/download-personal-data" element={<DownloadPersonalData />} />
        <Route path="account/email" element={<Email />} />
        <Route path="account/enable-authenticator" element={<EnableAuthenticator />} />
        <Route path="account/external-logins" element={<ExternalLogins />} />
        <Route path="account/generate-recovery-codes" element={<GenerateRecoveryCodes />} />
        <Route path="account/personal-data" element={<PersonalData />} />
        <Route path="account/reset-authenticator" element={<ResetAuthenticator />} />
        <Route path="account/set-password" element={<SetPassword />} />
        <Route path="account/show-recovery-codes" element={<ShowRecoveryCodes />} />
        <Route path="account/two-factor-authentication" element={<TwoFactorAuthentication />} />
      </Route>
    </Route>
  </Routes>
);

export default PlatformRoutes;
