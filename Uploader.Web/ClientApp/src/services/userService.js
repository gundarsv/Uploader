import { UserManager, WebStorageStateStore } from 'oidc-client';
import { removeUser, storeUser } from '../slices/authSlice';
import { setAuthHeader } from '../utils/axiosHeaders';

const config = {
    authority: "https://localhost:7281",
    client_id: "uploader.web",
    redirect_uri: "https://localhost:5001/signin-oidc",
    response_type: "id_token token",
    scope: "openid profile uploader.api uploader.web.api",
    post_logout_redirect_uri: "https://localhost:5001/signout-oidc",
    userStore: new WebStorageStateStore({ store: window.localStorage })
};

const userManager = new UserManager(config);

export async function loadUserFromStorage(store) {
    try {
        let user = await userManager.getUser()
        if (!user) {
            return store.dispatch(removeUser());
        }

        setAuthHeader(user.access_token);
        store.dispatch(storeUser({ name: user.profile.name, isAdministrator: user.profile.role === "Administrator" }))
    } catch (e) {
        store.dispatch(removeUser())
    }
}

export function signinRedirect() {
    return userManager.signinRedirect()
}

export function signinRedirectCallback() {
    return userManager.signinRedirectCallback()
}

export function signoutRedirect() {
    return userManager.signoutRedirect();
}

export function signoutRedirectCallback() {
    return userManager.signoutRedirectCallback()
}

export default userManager