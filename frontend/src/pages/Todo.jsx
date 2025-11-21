import {useEffect, useState} from 'react';
import {useNavigate, useSearchParams} from 'react-router-dom';
import {Button, Form, message, Space, Table, Tag, Tooltip} from 'antd';
import {DeleteOutlined, EditOutlined, PlusOutlined} from '@ant-design/icons';
import {dropdownAPI, todoAPI} from '../api';
import dayjs from 'dayjs';
import StatusSelector from '../components/StatusSelector';
import TodoModal from '../components/TodoModal';

function Todo()
{
  const navigate = useNavigate();
  const [searchParams, setSearchParams] = useSearchParams();
  const [todos, setTodos] = useState([]);
  const [loading, setLoading] = useState(false);
  const [statusOptions, setStatusOptions] = useState([]);
  const [form] = Form.useForm();
  const [modalVisible, setModalVisible] = useState(false);
  const [editingId, setEditingId] = useState(null);
  const [currentFilter, setCurrentFilter] = useState(null);
  const [user, setUser] = useState(null);

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (!token) {
      navigate('/login');
      return;
    }
    const userStr = localStorage.getItem('user');
    if (userStr) {
      try {
        const userData = JSON.parse(userStr);
        setUser(userData);
      } catch (e) {
        console.error('Error parsing user data:', e);
      }
    }
    loadDropdown();

    const statusParam = searchParams.get('status');
    if (statusParam) {
      setCurrentFilter(statusParam);
      loadTodos(statusParam);
    } else {
      loadTodos();
    }

    const id = searchParams.get('id');
    const create = searchParams.get('create');

    if (id) {
      setEditingId(parseInt(id));
      setModalVisible(true);
      loadTodoDetail(parseInt(id));
    } else {
      if (create === 'true') {
        setEditingId(null);
        form.resetFields();
        form.setFieldsValue({
          title: 'Công việc',
          status: statusOptions[0]?.value || 'InProgress',
        });
        setModalVisible(true);
      }
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    const id = searchParams.get('id');
    const create = searchParams.get('create');

    if (id) {
      const parsedId = parseInt(id);
      setEditingId(parsedId);
      setModalVisible(true);
      loadTodoDetail(parsedId);
    } else {
      if (create === 'true' && !editingId) {
        setEditingId(null);
        form.resetFields();
        form.setFieldsValue({
          status: statusOptions[0]?.value || 'InProgress',
        });
        setModalVisible(true);
      }
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [searchParams, statusOptions]);

  const loadDropdown = async () => {
    try {
      const response = await dropdownAPI.get('TodoStatus');
      const statusData = response.data.enums?.TodoStatus || [];
      setStatusOptions(statusData);
    } catch (error) {
      console.error(error);
    }
  };

  const loadTodos = async (status = null) => {
    setLoading(true);
    try {
      const params = {};
      if (status) {
        params.status = status;
      }
      const response = await todoAPI.list(params);
      setTodos(response.data.data.items || []);
    } catch (error) {
      if (error.response?.status === 401) {
        localStorage.removeItem('token');
        navigate('/login');
      }
      message.error('Lỗi khi tải danh sách');
    }
    finally {
      setLoading(false);
    }
  };

  const loadTodoDetail = async (id) => {
    try {
      const response = await todoAPI.detail(id);
      const todo = response.data.data;
      form.setFieldsValue({
        title: todo.title,
        dueDate: todo.dueDate ? dayjs(todo.dueDate) : null,
        status: todo.status,
      });
    } catch {
      message.error('Lỗi khi tải chi tiết');
    }
  };

  const handleCreate = () => {
    setEditingId(null);
    form.resetFields();
    form.setFieldsValue({
      title: 'Công việc',
      status: statusOptions[0]?.value || 'InProgress',
    });
    setSearchParams({create: 'true'});
    setModalVisible(true);
  };

  const handleEdit = (record) => {
    setEditingId(record.id);
    form.setFieldsValue({
      title: record.title,
      dueDate: record.dueDate ? dayjs(record.dueDate) : null,
      status: record.status,
    });
    setSearchParams({id: record.id.toString()});
    setModalVisible(true);
  };

  const handleDelete = async (id) => {
    try {
      await todoAPI.delete(id);
      message.success('Xóa thành công');
      loadTodos(currentFilter);
    } catch {
      message.error('Xóa thất bại');
    }
  };

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      const data = {
        title: values.title,
        dueDate: values.dueDate ? values.dueDate.toISOString() : null,
        status: values.status,
      };

      if (editingId) {
        await todoAPI.update(editingId, data);
        message.success('Cập nhật thành công');
      } else {
        await todoAPI.create(data);
        message.success('Tạo mới thành công');
      }

      setModalVisible(false);
      form.resetFields();
      setEditingId(null);
      const params = new URLSearchParams();
      if (currentFilter) {
        params.set('status', currentFilter);
      }
      setSearchParams(params);
      const url = currentFilter ? `/todo?status=${currentFilter}` : '/todo';
      window.history.replaceState({}, '', url);
      loadTodos(currentFilter);
    } catch (error) {
      if (error.errorFields) {
        return;
      }
      message.error(editingId ? 'Cập nhật thất bại' : 'Tạo mới thất bại');
      console.error(error);
    }
  };

  const handleStatusFilter = (value) => {
    setCurrentFilter(value);
    const newParams = new URLSearchParams(searchParams);
    if (value) {
      newParams.set('status', value);
    } else {
      newParams.delete('status');
    }
    setSearchParams(newParams);
    loadTodos(value);
  };

  const getStatusColor = (status) => {
    if (status === 'Completed') {
      return 'success';
    }
    if (status === 'InProgress') {
      return 'processing';
    }
    return 'default';
  };

  const columns = [
    {
      title: <span style={{fontSize: '16px', fontWeight: 'bold'}}>Tiêu đề</span>,
      dataIndex: 'title',
      key: 'title',
      render: (text) => <span style={{fontSize: '16px'}}>{text}</span>,
    },
    {
      title: <span style={{fontSize: '16px', fontWeight: 'bold'}}>Ngày hết hạn</span>,
      dataIndex: 'dueDate',
      key: 'dueDate',
      align: 'center',
      render: (date) => <span style={{fontSize: '16px'}}>{date ? dayjs(date).format('DD/MM/YYYY') : '-'}</span>,
    },
    {
      title: <span style={{fontSize: '16px', fontWeight: 'bold'}}>Trạng thái</span>,
      dataIndex: 'status',
      key: 'status',
      align: 'center',
      render: (status) => {
        const option = statusOptions.find(opt => opt.value === status);
        return (
            <Tag color={getStatusColor(status)} style={{fontSize: '16px', borderRadius: '999px', padding: '4px 12px'}}>
              {option ? option.label : status}
            </Tag>
        );
      },
    },
    {
      key: 'action',
      align: 'center',
      render: (_, record) => (
          <Space>
            <Tooltip title="Sửa">
              <Button
                  onClick={() => handleEdit(record)}
                  icon={<EditOutlined/>}
                  style={{borderRadius: '999px', width: '40px', height: '40px'}}
              />
            </Tooltip>
            <Tooltip title="Xóa">
              <Button
                  danger
                  onClick={() => handleDelete(record.id)}
                  icon={<DeleteOutlined/>}
                  style={{borderRadius: '999px', width: '40px', height: '40px'}}
              />
            </Tooltip>
          </Space>
      ),
    },
  ];

  return (
      <div style={{padding: '24px', maxWidth: '1200px', margin: '0 auto', fontSize: '16px'}}>
        <div style={{marginBottom: '24px', display: 'flex', justifyContent: 'space-between', alignItems: 'center'}}>
          <h1 style={{fontSize: '28px', margin: 0, marginBottom: '4px'}}>
            Hi, {user?.fullName || user?.name || 'User'}
          </h1>
          <Space>
            <StatusSelector
                value={currentFilter}
                onChange={handleStatusFilter}
                statusOptions={statusOptions}
                getStatusColor={getStatusColor}
                placeholder="Lọc theo trạng thái"
                closable={true}
            />
            <Tooltip title="Thêm mới">
              <Button
                  type="primary"
                  onClick={handleCreate}
                  icon={<PlusOutlined/>}
                  style={{borderRadius: '999px', width: '40px', height: '40px'}}
              />
            </Tooltip>
          </Space>
        </div>

        <Table
            dataSource={todos}
            columns={columns}
            rowKey="id"
            loading={loading}
            style={{borderRadius: '16px', overflow: 'hidden', fontSize: '16px'}}
        />

        <TodoModal
            open={modalVisible}
            editingId={editingId}
            form={form}
            statusOptions={statusOptions}
            getStatusColor={getStatusColor}
            onOk={handleSubmit}
            onCancel={() => {
              setModalVisible(false);
              setEditingId(null);
            }}
            currentFilter={currentFilter}
            setSearchParams={setSearchParams}
        />
      </div>
  );
}

export default Todo;

