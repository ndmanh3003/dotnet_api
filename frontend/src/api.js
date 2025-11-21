import axios from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

api.interceptors.response.use(
    (response) => response,
    (error) => {
      console.error('API Error:', error.response?.data || error.message);
      return Promise.reject(error);
    },
);

export const authAPI = {
  callback: (code) => api.post('/auth/callback', {code, redirectUri: '/login'}),
  me: () => api.get('/auth/me'),
};

export const todoAPI = {
  list: (params) => api.get('/todo', {params}),
  detail: (id) => api.get(`/todo/${id}`),
  create: (data) => api.post('/todo', data),
  update: (id, data) => api.put(`/todo/${id}`, data),
  delete: (id) => api.delete(`/todo/${id}`),
};

export const dropdownAPI = {
  get: (enums) => api.get('/dropdown', {params: {enums}}),
};

export default api;

