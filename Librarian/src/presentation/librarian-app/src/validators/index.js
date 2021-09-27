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
    newuser: {
        email: {
            required, email
        },
        firstName: {
            required, minLength: minLength(3), maxLength: maxLength(20)
        },
        surName: {
            required, minLength: minLength(3), maxLength: maxLength(20)
        },
        username: {
            required, minLength: minLength(5), maxLength: maxLength(20)
        },
        password: {
            required, minLength: minLength(8), maxLength: maxLength(20)
        },
        passwordagain: {
            required, minLength: minLength(8), maxLength: maxLength(20)
        }
    },
    addbook: {
        title: {
            required, maxLength: maxLength(50)
        },
        authors: {
            required, minLength: minLength(10), maxLength: maxLength(200)
        },
        publisher: {
            required, minLength: minLength(5), maxLength: maxLength(50)
        }
    }
};