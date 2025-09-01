import { get, post, put } from "../../../api";
import type {
  ForgotPasswordRequest,
  InfoRequest,
  LoginRequest,
  RefreshRequest,
  RegisterRequest,
  ResendConfirmationEmailRequest,
  ResetPasswordRequest,
  TwoFactorRequest,
 } from "../../../models/auth";
import type { IdentityUserModel } from "../../../models/identity-user-model";

export const postRegister = async (model: RegisterRequest, signal?: AbortSignal) =>
  post("/api/auth/register", model, signal);

export const postLogin = async (model: LoginRequest, signal?: AbortSignal) =>
  post("/api/auth/login", model, signal);

export const postRefresh = async (model: RefreshRequest, signal?: AbortSignal) =>
  post("/api/auth/refresh", model, signal);

export const getConfirmEmail = async (userId: string, code: string, changedEmail: string, signal?: AbortSignal) =>
  get(`/api/auth/confirmEmail?userId=${encodeURIComponent(userId)}&code=${encodeURIComponent(code)}&changedEmail=${encodeURIComponent(changedEmail)}`, signal);

export const postResendConfirmationEmail = async (model: ResendConfirmationEmailRequest, signal?: AbortSignal) =>
  post("/api/auth/resendConfirmationEmail", model, signal);

export const postForgotPassword = async (model: ForgotPasswordRequest, signal?: AbortSignal) =>
  post("/api/auth/forgotPassword", model, signal);

export const postResetPassword = async (model: ResetPasswordRequest, signal?: AbortSignal) =>
  post("/api/auth/resetPassword", model, signal);

export const postTwoFactor = async (model: TwoFactorRequest, signal?: AbortSignal) =>
  post("/api/auth/manage/2fa​", model, signal);

export const getInfo = async (signal?: AbortSignal) =>
  get("/api/auth/manage/info", signal);

export const postInfo = async (model: InfoRequest, signal?: AbortSignal) =>
  post("/api/auth/manage/info", model, signal);

// Extended
export const getUser = async (signal?: AbortSignal) =>
  get("/api/auth/user", signal);

export const putUser = async (model: IdentityUserModel<string>, signal?: AbortSignal) =>
  put("/api/auth/user", model, signal);
