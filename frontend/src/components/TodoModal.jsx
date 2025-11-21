import {DatePicker, Form, Input, Modal} from 'antd';
import dayjs from 'dayjs';
import StatusSelector from './StatusSelector';

const {useWatch} = Form;

const TodoModal = ({
  open,
  editingId,
  form,
  statusOptions,
  getStatusColor,
  onOk,
  onCancel,
  currentFilter,
  setSearchParams,
}) => {
  const statusValue = useWatch('status', form);

  const handleCancel = () => {
    form.resetFields();
    const params = new URLSearchParams();
    if (currentFilter) {
      params.set('status', currentFilter);
    }
    setSearchParams(params);
    const url = currentFilter ? `/todo?status=${currentFilter}` : '/todo';
    window.history.replaceState({}, '', url);
    onCancel();
  };

  return (
      <Modal
          title={editingId ? 'Chỉnh sửa công việc' : 'Tạo mới công việc'}
          open={open}
          onOk={onOk}
          onCancel={handleCancel}
          okText={editingId ? 'Cập nhật' : 'Tạo mới'}
          cancelText="Hủy"
          style={{borderRadius: '16px'}}
      >
        <Form form={form} layout="vertical" style={{marginTop: '20px', fontSize: '16px'}}>
          <Form.Item
              name="title"
              label={<span style={{fontSize: '16px'}}>Tiêu đề</span>}
              rules={[{required: true, message: 'Vui lòng nhập tiêu đề'}]}
              initialValue={editingId ? undefined : 'Công việc'}
          >
            <Input
                placeholder="Nhập tiêu đề"
                style={{borderRadius: '999px', fontSize: '16px', height: '40px'}}
            />
          </Form.Item>
          <Form.Item
              name="dueDate"
              label={<span style={{fontSize: '16px'}}>Ngày hết hạn</span>}
              initialValue={dayjs()}
          >
            <DatePicker
                style={{width: '100%', borderRadius: '999px', fontSize: '16px', height: '40px'}}
                format="DD/MM/YYYY"
            />
          </Form.Item>
          <Form.Item
              name="status"
              label={<span style={{fontSize: '16px'}}>Trạng thái</span>}
              rules={[{required: true, message: 'Vui lòng chọn trạng thái'}]}
              initialValue={statusOptions[0]?.value || 'InProgress'}
          >
            <StatusSelector
                value={statusValue}
                onChange={(value) => form.setFieldValue('status', value)}
                statusOptions={statusOptions}
                getStatusColor={getStatusColor}
                placeholder="Chọn trạng thái"
                closable={false}
            />
          </Form.Item>
        </Form>
      </Modal>
  );
};

export default TodoModal;

