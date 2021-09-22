import {
    required,
    email,
    minLength,
    maxLength
} from "vuelidate/lib/validators";

export default {
    login: {
        email: { required, email },
        password: { required, minLength: minLength(8), maxLength: maxLength(20) }
    },
    firstName: {
        required,
        minLength: minLength(3),
        maxLength: maxLength(20)
    },
    surName: {
        required,
        minLength: minLength(3),
        maxLength: maxLength(20)
    },
};