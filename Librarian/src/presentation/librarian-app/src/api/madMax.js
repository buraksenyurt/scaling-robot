import { getToken } from "@/auth/authService";

export function patrol(ctx) {
    ctx.interceptors.request.use(function(options) {
    const jwtToken = getToken();

    if (jwtToken) {
      options.headers["Authorization"] = `Bearer ${jwtToken}`;
    }

    return options;
  });

  ctx.interceptors.response.use(
    response => response,
    error =>
      Promise.reject(
        (error.response && error.response.data) || "Galiba bir şeyler ters gitti :|"
      )
  );

  return ctx;
}