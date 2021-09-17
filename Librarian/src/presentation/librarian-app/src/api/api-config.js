import axios from "axios";
import { patrol } from "@/api/madMax";
const debug = process.env.NODE_ENV !== "production";

/*
    Üretim ortamında değilse localhost'tan üretim ortamındaysak gerçek adresinden bağlanacağımız bir axios nesnesi üretiliyor
*/
const baseURL = debug
  ? "https://localhost:5001/api"
  : "https://mylibrary.somewhere/api";

let api = axios.create({ baseURL });
api = patrol(api);

export default api;