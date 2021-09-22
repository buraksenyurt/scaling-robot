import {
    required,
    email,
    minLength,
    maxLength
} from "vuelidate/lib/validators";

export default {
    login: {
        username: { required, minLength: minLength(5), maxLength: maxLength(20) },
        password: { required, minLength: minLength(8), maxLength: maxLength(20) }
    },
    email: {
        required,
        email
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