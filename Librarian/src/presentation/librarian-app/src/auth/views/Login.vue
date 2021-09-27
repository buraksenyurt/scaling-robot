<template>
    <v-app>
        <v-dialog v-model="dialog"
                  persistent
                  max-width="600px"
                  min-width="360px"
                  @click:outside="navigateHome">
            <div>
                <v-tabs show-arrows
                        background-color="pink accent-4"
                        icons-and-text
                        dark
                        grow>
                    <v-tabs-slider color="pink darken-4"></v-tabs-slider>
                    <v-tab>
                        <v-icon large>mdi-login</v-icon>
                        <div>Giriş</div>
                    </v-tab>
                    <v-tab>
                        <v-icon large>mdi-account-box-outline</v-icon>
                        <div>Kayıt</div>
                    </v-tab>
                    <v-tab-item>
                        <v-card class="px-4">
                            <v-card-text>
                                <form @submit.prevent="onSubmit">
                                    <v-row>
                                        <v-col cols="12">
                                            <v-text-field label="Kullanıcı Adın"
                                                          v-model="login.username"
                                                          @input="$v.login.username.$touch()"
                                                          @blur="$v.login.username.$touch()"
                                                          :error-messages="usernameErrors"></v-text-field>
                                        </v-col>
                                        <v-col cols="12">
                                            <v-text-field type="password"
                                                          label="Şifre"
                                                          hint="Şöyle güçlü bir şifren olmalı"
                                                          counter
                                                          v-model="login.password"
                                                          @input="$v.login.password.$touch()"
                                                          @blur="$v.login.password.$touch()"
                                                          :error-messages="passwordErrors"></v-text-field>
                                        </v-col>
                                        <v-col class="d-flex" cols="12" sm="6" xsm="12"> </v-col>
                                        <v-spacer></v-spacer>
                                        <v-col class="d-flex" cols="12" sm="3" xsm="12" align-end>
                                            <v-btn :disabled="false" color="primary" type="submit">Dene</v-btn>
                                        </v-col>
                                    </v-row>
                                </form>
                            </v-card-text>
                        </v-card>
                    </v-tab-item>
                    <v-tab-item>
                        <v-card class="px-4">
                            <v-card-text>
                                <div>
                                    <v-row>
                                        <v-col cols="12" sm="6" md="6">
                                            <v-text-field label="Adın"
                                                          v-model="register.firstname"
                                                          @input="$v.register.firstname.$touch()"
                                                          @blur="$v.register.firstname.$touch()"
                                                          :error-messages="registerFirstnameErrors"
                                                          maxlength="20"
                                                          minlength="3"
                                                          required></v-text-field>
                                        </v-col>
                                        <v-col cols="12" sm="6" md="6">
                                            <v-text-field label="Soyadın"
                                                          v-model="register.surname"
                                                          @input="$v.register.surname.$touch()"
                                                          @blur="$v.register.surname.$touch()"
                                                          :error-messages="registerSurnameErrors"
                                                          maxlength="20"
                                                          minlength="3"
                                                          required></v-text-field>
                                        </v-col>
                                        <v-col cols="12" sm="6" md="6">
                                            <v-text-field label="Birde kullanıcı adın ;)"
                                                          v-model="register.username"
                                                          @input="$v.register.username.$touch()"
                                                          @blur="$v.register.username.$touch()"
                                                          :error-messages="registerUsernameErrors"
                                                          maxlength="20"
                                                          required></v-text-field>
                                        </v-col>
                                        <v-col cols="12">
                                            <v-text-field label="E-Posta Adresin"
                                                          @input="$v.register.email.$touch()"
                                                          @blur="$v.register.email.$touch()"
                                                          :error-messages="registerEmailErrors"
                                                          required></v-text-field>
                                        </v-col>
                                        <v-col cols="12">
                                            <v-text-field label="Şifre"
                                                          type="password"
                                                          hint="Şöyle kuvvetli bir şifre belirlesek ya."
                                                          v-model="register.password"
                                                          @input="$v.register.password.$touch()"
                                                          @blur="$v.register.password.$touch()"
                                                          :error-messages="registerPasswordErrors"
                                                          counter></v-text-field>
                                        </v-col>
                                        <v-col cols="12">
                                            <v-text-field block
                                                          label="Şifre(Yeniden)"
                                                          type="password"
                                                          v-model="register.passwordagain"
                                                          @input="$v.register.passwordagain.$touch()"
                                                          @blur="$v.register.passwordagain.$touch()"
                                                          :error-messages="registerPasswordErrors"
                                                          counter></v-text-field>
                                        </v-col>
                                        <v-spacer></v-spacer>
                                        <v-col class="d-flex ml-auto" cols="12" sm="3" xsm="12">
                                            <v-btn :disabled="$v.$anyError" color="primary">Kaydol</v-btn>
                                        </v-col>
                                    </v-row>
                                </div>
                            </v-card-text>
                        </v-card>
                    </v-tab-item>
                </v-tabs>
            </div>
        </v-dialog>
    </v-app>
</template>

<script>
    import { mapActions } from "vuex";
    import router from "@/router";
    import validators from "@/validators";
    export default {
        name: "Login",
        data: () => ({
            dialog: true,
            tab: null,
            login: {
                username: "",
                password: "",
            },
            register: {
                firstname: "",
                surname: "",
                email: "",
                username: "",
                password: "",
                passwordagain: ""
            }
        }),
        methods: {
            ...mapActions("authModule", ["loginUserAction"]),
            onSubmit() {
                this.loginUserAction(this.login).then(() => {
                    this.$router.push({ path: "/dashboard" });
                });
            },
            navigateHome() {
                router.push("/");
            },
        },
        computed: {
            usernameErrors() {
                const errors = [];
                if (!this.$v.login.username.$dirty) return errors;
                !this.$v.login.username.minLength &&
                    errors.push("En az 5 karakter olmalıydı.");
                !this.$v.login.username.maxLength &&
                    errors.push("Paranoyak olma. En fazla 20 karakter.");
                !this.$v.login.username.required && errors.push("Lütfen kullanıcı adınızı giriniz");
                return errors;
            },
            passwordErrors() {
                const errors = [];
                if (!this.$v.login.password.$dirty) return errors;
                !this.$v.login.password.required && errors.push("Şifre girilmeli");
                !this.$v.login.password.minLength &&
                    errors.push("En az 8 karakter olsun");
                !this.$v.login.password.maxLength &&
                    errors.push("Paranoyak olma. En fazla 20 karakter.");
                return errors;
            },
            registerEmailErrors() {
                const errors = [];
                if (!this.$v.register.email.$dirty) return errors;
                !this.$v.register.email.email && errors.push("Beni kandırma. Düzgün bir e-posta gir.");
                !this.$v.register.email.required && errors.push("e-posta girilmesi lazım");
                return errors;
            },
            registerFirstnameErrors() {
                const errors = [];
                if (!this.$v.register.firstname.$dirty) return errors;
                !this.$v.register.firstname.minLength &&
                    errors.push("En az 3 karakter olmalıydı.");
                !this.$v.register.firstname.maxLength &&
                    errors.push("Paranoyak olma. En fazla 20 karakter.");
                !this.$v.register.firstname.required && errors.push("Lütfen adınızı giriniz");
                return errors;
            },
            registerSurnameErrors() {
                const errors = [];
                if (!this.$v.register.surname.$dirty) return errors;
                !this.$v.register.surname.minLength &&
                    errors.push("En az 3 karakter olmalıydı.");
                !this.$v.register.surname.maxLength &&
                    errors.push("Paranoyak olma. En fazla 20 karakter.");
                !this.$v.register.surname.required && errors.push("Lütfen soyadınızı giriniz");
                return errors;
            },
            registerUsernameErrors() {
                const errors = [];
                if (!this.$v.register.username.$dirty) return errors;
                !this.$v.register.username.minLength &&
                    errors.push("En az 5 karakter olmalıydı.");
                !this.$v.register.username.maxLength &&
                    errors.push("Paranoyak olma. En fazla 20 karakter.");
                !this.$v.register.username.required && errors.push("Lütfen kullanıcı adınızı giriniz");
                return errors;
            },
            registerPasswordErrors() {
                const errors = [];
                if (!this.$v.register.password.$dirty) return errors;
                !this.$v.register.password.required && errors.push("Şifre girilmeli");
                !this.$v.register.password.minLength &&
                    errors.push("En az 8 karakter olsun");
                !this.$v.register.password.maxLength &&
                    errors.push("Paranoyak olma. En fazla 20 karakter.");
                return errors;
            },
        },
        validations: {
            login: validators.login,
            register: {
                firstname: validators.newuser.firstName,
                surname: validators.newuser.surName,
                email: validators.newuser.email,
                username: validators.newuser.username,
                password: validators.newuser.password,
                passwordagain: validators.newuser.passwordagain
            }
        }
    };
</script>