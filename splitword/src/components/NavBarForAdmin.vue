<template>
  <div>
    <div class="line"></div>
    <el-menu
        router
        :default-active="activeIndex"
        class="el-menu-demo"
        mode="horizontal"
        @select="handleSelect"
        background-color="#545c64"
        text-color="#fff"
        active-text-color="#ffd04b"
    >
      <el-menu-item  index="/AllUser">用户管理</el-menu-item>
      <el-menu-item  index="/AllFile">知识分类管理</el-menu-item>
    </el-menu>
  </div>
</template>

<script>
export default {
  name: 'NavBarForAdmin',
  data() {
    return {
      activeIndex: '/Main',
      activeIndex2: '/Main'
    };
  },
  methods: {
    handleSelect(key, keyPath) {
      console.log(key, keyPath);
    }
  }
}
</script>