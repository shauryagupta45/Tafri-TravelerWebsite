package com.prologin.login.Controllers;

import com.prologin.login.Models.Admin;
import com.prologin.login.Repositories.AdminRepository;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpSession;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

import java.util.HashMap;
import java.util.Map;
import java.util.Optional;

@RestController
public class Login {

    @Autowired
    private AdminRepository adminRepository;

    @PostMapping("/login")
    public Map<String, String> loginAdmin(@RequestBody Map<String, String> loginData, HttpServletRequest request) {
        String username = loginData.get("username");
        String password = loginData.get("password");

        Optional<Admin> admin = adminRepository.findByUsername(username);
        Map<String, String> response = new HashMap<>();

        if (admin.isPresent() && admin.get().getPassword().equals(password)) {
            HttpSession session = request.getSession();
            session.setAttribute("admin", admin.get());
            session.setMaxInactiveInterval(3600); // 1 hour

            response.put("message", "Admin logged in successfully");
            response.put("status", "true");
            response.put("redirectUrl", "/dashboard.jsp");
        } else {
            response.put("message", "Invalid username or password");
            response.put("status", "false");
            response.put("redirectUrl", "/login.jsp");
        }

        return response;
    }

}
