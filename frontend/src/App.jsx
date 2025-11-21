import {BrowserRouter, Navigate, Route, Routes} from 'react-router-dom';
import {ConfigProvider} from 'antd';
import Login from './pages/Login';
import Todo from './pages/Todo';

function App()
{
  return (
      <ConfigProvider
          theme={{
            token: {
              fontSize: 16,
            },
          }}
      >
        <BrowserRouter>
          <Routes>
            <Route path="/login" element={<Login/>}/>
            <Route path="/todo" element={<Todo/>}/>
            <Route path="/" element={<Navigate to="/todo" replace/>}/>
          </Routes>
        </BrowserRouter>
      </ConfigProvider>
  );
}

export default App;
